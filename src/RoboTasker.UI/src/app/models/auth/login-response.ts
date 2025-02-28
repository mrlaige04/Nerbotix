import {AccessToken} from './access-token';
import {CurrentUser} from '../user/current-user';

export interface LoginResponse {
  token: AccessToken;
  user: CurrentUser;
}
