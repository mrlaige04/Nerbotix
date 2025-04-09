import {Guid} from 'guid-typescript';
import {CreateCapabilityItemRequest} from './create-capability-request';

export interface UpdateCapabilityRequest {
  name?: string | undefined;
  description?: string | undefined;
  deletedCapabilities?: Guid[] | undefined;
  newCapabilities?: CreateCapabilityItemRequest[] | undefined;
}
