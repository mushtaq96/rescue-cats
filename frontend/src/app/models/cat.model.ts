export interface Cat {
  id: string;
  name: string;
  breed: string;
  age: number;
  description: string;
  imageUrl: string;
  isAdopted: boolean;
  goodWithKids: boolean;
  goodWithDogs: boolean;
  playfulness: number; // Scale of 1-5
  location: {
    street: string;
    city: string;
    state: string;
    postalCode: string;
    latitude: number;
    longitude: number;
  };
}

export interface AdoptionApplication {
  id: string;
  userId: string;
  fullName: string;
  catId: string;
  status: string;
  email: string;
  phoneNo: string;
  createdAt: string;
  updatedAt: string;
  location: {
    street: string;
    city: string;
    state: string;
    postalCode: string;
    latitude: number;
    longitude: number;
  };
  reason: string;
}