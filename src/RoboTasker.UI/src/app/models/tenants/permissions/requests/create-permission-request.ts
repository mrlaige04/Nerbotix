export interface CreatePermissionRequest {
  name: string;
  permissions: CreatePermissionRequestItem[];
}

export interface CreatePermissionRequestItem {
  name: string;
}
