import {inject, Injectable, output} from '@angular/core';
import {BaseHttp} from '../base/base-http';
import {CreateChatRequest} from '../../models/chatting/requests/create-chat-request';
import {Observable, Subject} from 'rxjs';
import {ChatBase} from '../../models/chatting/chat-base';
import {PaginationRequest} from '../../models/common/pagination-request';
import {PaginatedList} from '../../models/common/paginated-list';
import {Guid} from 'guid-typescript';
import {Success} from '../../models/success';
import {Message} from '../../models/chatting/message';
import {ChatInfo} from '../../models/chatting/chat-info';

@Injectable({
  providedIn: 'root'
})
export class ChatService {
  private base = inject(BaseHttp);
  private readonly baseUrl = 'chats';

  public chatLinkUpdated = new Subject<{
    id: Guid,
    lastMessage: string,
    updatedAt: Date
  }>();

  public refreshChatList = new Subject<void>();

  createChat(data: CreateChatRequest): Observable<ChatBase> {
    const url = this.baseUrl;
    return this.base.post<CreateChatRequest, ChatBase>(url, data);
  }

  getChatInfo(id: Guid): Observable<ChatInfo> {
    const url = `${this.baseUrl}/${id}`;
    return this.base.get<ChatInfo>(url);
  }

  getChats(data: PaginationRequest) : Observable<PaginatedList<ChatBase>> {
    const url = this.baseUrl;
    return this.base.get<PaginatedList<ChatBase>>(url, { ...data });
  }

  getMessages(chatId: Guid, data: PaginationRequest): Observable<PaginatedList<Message>> {
    const url = `${this.baseUrl}/${chatId}/messages`;
    return this.base.get<PaginatedList<Message>>(url, { ...data });
  }

  deleteChat(id: Guid): Observable<Success> {
    const url = `${this.baseUrl}/${id}`;
    return this.base.delete<Success>(url);
  }
}
