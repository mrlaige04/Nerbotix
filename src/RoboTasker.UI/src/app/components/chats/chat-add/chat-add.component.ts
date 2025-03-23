import {Component, computed, DestroyRef, inject, OnInit, signal} from '@angular/core';
import {BaseComponent} from '../../common/base/base.component';
import {UsersService} from '../../../services/users/users.service';
import {DynamicDialogRef} from 'primeng/dynamicdialog';
import {UserBase} from '../../../models/users/user-base';
import {catchError, finalize, of, tap} from 'rxjs';
import {takeUntilDestroyed} from '@angular/core/rxjs-interop';
import {Button} from 'primeng/button';
import {MultiSelect} from 'primeng/multiselect';
import {FormsModule} from '@angular/forms';
import {ChatService} from '../../../services/chatting/chat.service';
import {HttpErrorResponse} from '@angular/common/http';
import {InputText} from 'primeng/inputtext';

@Component({
  selector: 'rb-chat-add',
  imports: [
    Button,
    MultiSelect,
    FormsModule,
    InputText
  ],
  templateUrl: './chat-add.component.html',
  styleUrl: './chat-add.component.scss'
})
export class ChatAddComponent extends BaseComponent implements OnInit {
  private usersService = inject(UsersService);
  private dialogRef = inject(DynamicDialogRef<ChatAddComponent>);
  private destroyRef = inject(DestroyRef);
  private chatService = inject(ChatService);

  private allUsers = signal<UserBase[]>([]);
  availableUsers = computed(() => {
    const currentUserId = this.currentUserService.currentUser()?.id;
    return this.allUsers().filter(u => u.id !== currentUserId);
  });

  name = signal<string | undefined>(undefined);
  selectedUsers = signal<UserBase[]>([]);

  pageNumber = signal<number>(1);
  pageSize = signal<number>(9999);

  ngOnInit() {
    this.usersService.getUsers({
      pageNumber: this.pageNumber(),
      pageSize: this.pageSize()
    }).pipe(
      tap((result) => {
        this.allUsers.set(result.items);
      }),
      takeUntilDestroyed(this.destroyRef)
    ).subscribe();
  }

  submit() {
    if (this.selectedUsers().length < 1) {
      return;
    }

    this.showLoader();
    this.chatService.createChat({
      name: this.name(),
      users: this.selectedUsers().map(u => u.id)
    }).pipe(
      catchError((error: HttpErrorResponse) => {
        const detail = error.error.detail;
        this.notificationService.showError('Error while creating a chat', detail);
        return of(null);
      }),
      tap((res) => {
        if (res) {
          this.dialogRef.close(true);
        }
      }),
      takeUntilDestroyed(this.destroyRef),
      finalize(() => this.hideLoader())
    ).subscribe();
  }
}
