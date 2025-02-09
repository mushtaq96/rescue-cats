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
}

export interface AdoptionApplication {
  id: string;
  catId: string;
  applicantName: string;
  email: string;
  phone: string;
  address: string;
  hasOtherPets: boolean;
  otherPetsDescription?: string;
  hasChildren: boolean;
  childrenAges?: string;
  reasonForAdoption: string;
  status: 'pending' | 'approved' | 'rejected';
  submittedAt: Date;
}