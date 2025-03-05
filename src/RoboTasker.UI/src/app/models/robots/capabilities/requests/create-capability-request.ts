export interface CreateCapabilityRequest {
  name: string;
  description?: string | undefined;
  capabilities: CreateCapabilityItemRequest[];
}

export interface CreateCapabilityItemRequest {
  name: string;
  description?: string | undefined;
}
