import { ViewUserRequest } from '../appuser/viewUserRequest';

export interface AuthResponse {
  id: string;
  userName: string;
  email: string;
  token: string;
  isSuccess: boolean;
  message: string;
  userDetails: ViewUserRequest; // Add this property to store user details
}
