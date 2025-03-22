import {Component, inject, OnDestroy, OnInit} from '@angular/core';
import {LayoutService} from '../../../services/layout/layout.service';

@Component({
  selector: 'rb-chats-wrapper',
  imports: [],
  templateUrl: './chats-wrapper.component.html',
  styleUrl: './chats-wrapper.component.scss'
})
export class ChatsWrapperComponent implements OnInit, OnDestroy {
  private layoutService = inject(LayoutService);

  ngOnInit() {
    this.layoutService.wrapToCard.set(false);
  }

  ngOnDestroy() {
    this.layoutService.wrapToCard.set(true);
  }
}
