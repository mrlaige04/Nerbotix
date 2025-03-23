import {Routes} from '@angular/router';
import {ChatsWrapperComponent} from './chats-wrapper/chats-wrapper.component';
import {ChatComponent} from './chat/chat.component';

export const CHATS_ROUTES: Routes = [
  {
    path: '',
    component: ChatsWrapperComponent,
    title: 'Chats',
    children: [
      {
        path: ':id',
        component: ChatComponent
      }
    ]
  }
];
