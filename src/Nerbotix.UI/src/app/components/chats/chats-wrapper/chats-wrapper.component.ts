import {Component, DestroyRef, inject, OnDestroy, OnInit, signal, ViewChild} from '@angular/core';
import {LayoutService} from '../../../services/layout/layout.service';
import {RouterLink, RouterOutlet} from '@angular/router';
import {Card} from 'primeng/card';
import {Divider} from 'primeng/divider';
import {Button} from 'primeng/button';
import {Avatar} from 'primeng/avatar';
import {ChatService} from '../../../services/chatting/chat.service';
import {BaseComponent} from '../../common/base/base.component';
import {catchError, finalize, of, tap} from 'rxjs';
import {takeUntilDestroyed} from '@angular/core/rxjs-interop';
import {ChatBase} from '../../../models/chatting/chat-base';
import {DynamicDialogRef} from 'primeng/dynamicdialog';
import {ChatAddComponent} from '../chat-add/chat-add.component';
import {ContextMenu} from 'primeng/contextmenu';
import { MenuItem } from 'primeng/api';
import {Guid} from 'guid-typescript';
import {HttpErrorResponse} from '@angular/common/http';
import { NgTemplateOutlet} from '@angular/common';
import {HasPermissionDirective} from '../../../utils/directives/has-permission.directive';
import {PermissionsNames} from '../../../models/tenants/permissions/permissions-names';
import {IsSuperAdminDirective} from '../../../utils/directives/is-super-admin.directive';
import {Drawer} from 'primeng/drawer';

@Component({
  selector: 'nb-chats-wrapper',
  imports: [
    RouterOutlet,
    Card,
    Divider,
    Button,
    RouterLink,
    Avatar,
    ContextMenu,
    HasPermissionDirective,
    IsSuperAdminDirective,
    NgTemplateOutlet,
    Drawer
  ],
  templateUrl: './chats-wrapper.component.html',
  styleUrl: './chats-wrapper.component.scss'
})
export class ChatsWrapperComponent extends BaseComponent implements OnInit, OnDestroy {
  private layoutService = inject(LayoutService);
  private chatsService = inject(ChatService);
  private destroyRef = inject(DestroyRef);
  private dialogRef: DynamicDialogRef<ChatAddComponent> | undefined;

  chats = signal<ChatBase[]>([]);
  currentChatId = signal<Guid | null>(null);

  pageNumber = signal<number>(1);
  pageSize = signal<number>(10);

  sideBarListOpened = signal<boolean>(false);

  isDesktop: boolean = this.layoutService.isDesktop;

  ngOnInit() {
    this.layoutService.wrapToCard.set(false);
    this.getChats();

    this.chatsService.chatLinkUpdated.pipe(
      takeUntilDestroyed(this.destroyRef)
    ).subscribe((chat) => {
      const existingChat = this.chats().find(c => c.id === chat.id);
      if (!existingChat) {
        return;
      }

      const filteredChats = this.chats().filter(c => c.id !== chat.id);
      existingChat!.lastMessage = chat.lastMessage;
      existingChat!.updatedAt = chat.updatedAt;
      this.chats.set([existingChat!, ...filteredChats]);
    });

    this.chatsService.refreshChatList.pipe(
      takeUntilDestroyed(this.destroyRef)
    ).subscribe(() => this.getChats());
  }

  private getChats() {
    this.showLoader();
    this.chatsService.getChats({
      pageNumber: this.pageNumber(),
      pageSize: this.pageSize(),
    }).pipe(
      tap((result) => {
        this.chats.set(result.items.sort((a,b) => a.updatedAt! < b.updatedAt! ? 1 : -1));
      }),
      takeUntilDestroyed(this.destroyRef),
      finalize(() => this.hideLoader())
    ).subscribe();
  }

  createChat() {
    this.dialogRef = this.dialogService.open(ChatAddComponent, {
      modal: true,
      header: 'Add a new chat',
      style: {
        minWidth: '30%'
      },
      resizable: true,
      closable: true
    });

    this.dialogRef.onClose.pipe(
      tap(result => {
        if (result === true) {
          this.getChats();
        }
      }),
      takeUntilDestroyed(this.destroyRef)
    ).subscribe();
  }

  goToChatList() {
    this.currentChatId.set(null);
    this.router.navigate(['chat']);
  }

  goToChat(chat: ChatBase) {
    if (chat.id !== this.currentChatId()) {
      this.currentChatId.set(chat.id);
      this.sideBarListOpened.set(false);
      this.router.navigateByUrl('chats', { skipLocationChange: true }).then(() => {
        this.router.navigate(['chats', chat.id]);
      });
    }
  }

  calculateLastMessageOnChat(chat: ChatBase) {
    if (!chat.updatedAt) return;

    const now = new Date();
    const updatedAt = new Date(chat.updatedAt);
    const diffMs = now.getTime() - updatedAt.getTime();

    const seconds = Math.floor(diffMs / 1000);
    const minutes = Math.floor(seconds / 60);
    const hours = Math.floor(minutes / 60);
    const days = Math.floor(hours / 24);

    if (days > 0) return `${days}d`;
    if (hours > 0) return `${hours}h`;
    if (minutes > 0) return `${minutes}m`
    return 'Now';
  }

  ngOnDestroy() {
    this.layoutService.wrapToCard.set(true);
  }

  protected readonly PermissionsNames = PermissionsNames;
}
