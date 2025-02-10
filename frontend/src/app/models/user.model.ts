export interface User {
  id: number;
  email: string;
  details: {
    firstName: string;
    lastName: string;
  };
  location: {
    street: string;
    city: string;
    state: string;
    postalCode: string;
    latitude: number;
    longitude: number;
  };
  isVerified: boolean;
  verificationToken?: string;
  tokenExpiresAt?: Date;
  password: string;
  role?: 'user' | 'admin';
}

export interface LoginCredentials {
  email: string;
  password: string;
}

export interface SignupCredentials extends LoginCredentials {
  firstName: string;
  lastName: string;
}