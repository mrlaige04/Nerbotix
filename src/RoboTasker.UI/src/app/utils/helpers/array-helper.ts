export class ArrayHelper {
  public static CreateRange(min: number, max: number) {
    return Array.from({ length: max - min + 1 }, (_, i) => min + i);
  }
}
