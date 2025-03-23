import {
  AfterViewChecked,
  Component,
  DestroyRef,
  ElementRef,
  Inject,
  inject,
  OnDestroy,
  OnInit,
  signal,
  ViewChild
} from '@angular/core';
import {Card} from 'primeng/card';
import {BaseComponent} from '../../common/base/base.component';
import {ChatService} from '../../../services/chatting/chat.service';
import {Guid} from 'guid-typescript';
import {InputText} from 'primeng/inputtext';
import {Button} from 'primeng/button';
import {Avatar} from 'primeng/avatar';
import {Divider} from 'primeng/divider';
import {HubConnection, HubConnectionBuilder, LogLevel} from '@microsoft/signalr';
import {apiConfig, ApiConfig} from '../../../config/http.config';
import {Message} from '../../../models/chatting/message';
import {FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators} from '@angular/forms';
import {AuthService} from '../../../services/auth/auth.service';
import {takeUntilDestroyed} from '@angular/core/rxjs-interop';
import {tap} from 'rxjs';
import {DatePipe} from '@angular/common';
import {ContextMenu} from 'primeng/contextmenu';
import {MenuItem} from 'primeng/api';

@Component({
  selector: 'rb-chat',
  imports: [
    Card,
    InputText,
    Button,
    Avatar,
    Divider,
    FormsModule,
    DatePipe,
    ContextMenu,
    ReactiveFormsModule
  ],
  templateUrl: './chat.component.html',
  styleUrl: './chat.component.scss'
})
export class ChatComponent extends BaseComponent implements OnInit, AfterViewChecked, OnDestroy {
  private chatService = inject(ChatService);
  private connection?: HubConnection;
  private authService = inject(AuthService);
  private destroyRef = inject(DestroyRef);
  private wasScrolled = false;

  currentUserId = this.currentUserService.currentUser()?.id;

  constructor(@Inject(apiConfig) private apiConfig: ApiConfig) {
    super();
  }

  form = new FormGroup({
    input: new FormControl('', [Validators.required]),
  });

  @ViewChild('msgWrapper') msgWrapper!: ElementRef;

  messages = signal<Message[]>([]);

  chatId = signal<Guid | null>(null);
  chat = signal<any | null>(null);

  selectedMessage = signal<Message | null>(null);

  typingUser = signal<string | null>(null);

  @ViewChild('cm') cm?: ContextMenu;

  cmItems: MenuItem[] = [
    {
      label: 'Delete',
      icon: 'pi pi-trash',
      command: () => {
        if (this.selectedMessage()) {
          this.deleteMessage(this.selectedMessage()!.id, this.chatId()!);
        }
      }
    }
  ];

  ngOnInit() {
    const id = this.activatedRoute.snapshot.params['id'];
    if (!id) {
      this.router.navigate(['chats']);
      return;
    }

    this.chatId.set(id);
    const chatConnectionUrl = `${this.apiConfig.url}/chat?chatId=${id}`;
    const accessToken = this.authService.accessToken();
    this.connection = new HubConnectionBuilder()
      .withUrl(chatConnectionUrl, {
        headers: {
          Authorization: `${accessToken?.tokenType} ${accessToken?.accessToken}`
        }
      })
      .withAutomaticReconnect()
      .configureLogging(LogLevel.Critical)
      .build();

    this.connection.on('receive-message', this.onReceiveMessage.bind(this));
    this.connection.on('delete-message', this.onDeleteMessage.bind(this));

    this.connection.start()
      .then(() => console.log('connection started'))
      .catch((e) => console.log(e));

    this.loadMessages();
  }

  private loadMessages() {
    this.chatService.getMessages(this.chatId()!, {
      pageNumber: 1,
      pageSize: 9999
    }).pipe(
      tap((messages) => {
        this.messages.set(messages.items);
        this.wasScrolled = false;
      }),
      takeUntilDestroyed(this.destroyRef)
    ).subscribe();
  }

  sendMessage() {
    const message = this.form.value.input?.trim() ?? '';

    if (message.trim().length === 0) {
      return;
    }

    this.connection?.invoke('SendMessage', {
      chatId: this.chatId(),
      message
    });

    this.form.reset();
    this.scrollToBottom();
  }

  ngAfterViewChecked() {
    if (!this.wasScrolled) {
      this.scrollToBottom();
      this.wasScrolled = true;
    }
  }

  onCmOpen(message: Message, event: Event) {
    if (message.senderId === this.currentUserId) {
      this.selectedMessage.set(message);
      this.cm?.show(event);
    }
  }

  onCmHide() {
    this.selectedMessage.set(null);
  }

  deleteMessage(messageId: Guid, chatId: Guid) {
    this.connection?.invoke('DeleteMessage', {
      messageId, chatId
    });
  }

  private onReceiveMessage(message: Message) {
    this.messages.update(m => [...m, message]);
    this.wasScrolled = false;
    this.chatService.chatLinkUpdated.next({
      id: this.chatId()!,
      lastMessage: message.message,
      updatedAt: new Date()
    });
  }

  private onDeleteMessage(id: Guid) {
    this.messages.update(u => u.filter(m => m.id !== id));
  }

  private scrollToBottom() {
    this.msgWrapper.nativeElement.scrollTop = this.msgWrapper.nativeElement.scrollHeight;
  }

  ngOnDestroy() {
    this.connection?.stop();
  }

  calculateShowDayOfMessage(prevMessage: Message | null | undefined, message: Message) {
    if (!prevMessage) return true;

    let prevDay = prevMessage.updatedAt;
    let curDay = message.updatedAt;
    if (!prevDay || !curDay) return true;

    prevDay = new Date(prevDay);
    curDay = new Date(curDay);

    const prevMonth = prevDay.getMonth();
    const prevDate = prevDay.getDate();
    const curMonth = curDay.getMonth();
    const curDate = curDay.getDate();

    return prevMonth !== curMonth || prevDate !== curDate;
  }
}
