import {CategoryPropertyType} from '../../models/robots/categories/category-property-type';

export class PropertyTypeHelper {
  public static ConvertToExactType(type: CategoryPropertyType, value: string) : boolean | string | number | Date {
    switch (type) {
      case CategoryPropertyType.Boolean:
        return value.toLowerCase() == 'true'; // TODO: investigate how can this be fixed to normal comparison
      case CategoryPropertyType.String:
        return String(value);
      case CategoryPropertyType.Number:
        return isNaN(+value) ? 0.0 : +value;
      case CategoryPropertyType.DateTime:
        return new Date(value);
    }
  }
}
