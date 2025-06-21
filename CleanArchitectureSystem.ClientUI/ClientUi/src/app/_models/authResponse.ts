import { User } from './user';

export interface AuthResponse {
  id: string;
  userName: string;
  email: string;
  token: string;
  isSuccess: boolean;
  message: string;
  userDetails: User; // Add this property to store user details
}
