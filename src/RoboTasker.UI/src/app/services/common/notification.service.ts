import {inject, Injectable} from '@angular/core';
import {MessageService} from 'primeng/api';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private messageService = inject(MessageService);

  public showSuccess(title: string, message?: string) {
    this.show('success', title, message);
  }

  public showInfo(title: string, message?: string) {
    this.show('info', title, message);
  }

  public showWarning(title: string, message?: string) {
    this.show('warn', title, message);
  }

  public showError(title: string, message?: string) {
    this.show('error', title, message);
  }

  private show(severity: string, title: string, message?: string) {
    this.messageService.add({
      severity,
      summary: title,
      detail: message
    });
  }
}
