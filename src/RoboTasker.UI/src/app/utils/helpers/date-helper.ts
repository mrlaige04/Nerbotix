export class DateHelper {
  public static NormalizeDuration(duration: string) {
    const regex = /^(\d+):([0-5]\d)$/;
    const match = duration.match(regex);

    if (!match) {
      throw new Error('Invalid duration format.');
    }

    let hours = parseInt(match[1], 10);
    const minutes = match[2];

    const days = Math.floor(hours / 24);
    hours = hours % 24;

    return `${days.toString().padStart(2, '0')}:${hours.toString().padStart(2, '0')}:${minutes}`;
  }
}
