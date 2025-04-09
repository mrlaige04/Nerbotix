export interface IAuditableEntity {
  createdAt: Date;
  updatedAt?: Date | undefined;
}
