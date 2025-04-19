import {LogLevel} from '@microsoft/signalr';

export class Log {
  logLevel!: LogLevel;
  message?: string;
  timestamp!: Date;
}
