export class EnumHelper {
  public static toArray<T extends Record<string, string | number>>(enumObj: T) : { key: number; label: string }[] {
    return Object.entries(enumObj)
      .filter(([key, value]) => typeof value === "number")
      .map(([key, value]) => ({ key: value as number, label: key }));
  }

  public static getValueByName<T>(obj: T, name: string): T[keyof T] {
    return obj[name as keyof T];
  }
}
