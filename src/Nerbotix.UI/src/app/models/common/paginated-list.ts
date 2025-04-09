export interface PaginatedList<T> {
  pageNumber: number;
  pageSize: number;
  totalItems: number;
  items: T[];
}
