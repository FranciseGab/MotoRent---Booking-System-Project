export interface MotorResponseDto {
  id: number;
  brand: string;
  pricePerDay: number;
  isAvailable: boolean;
  imageUrl: string; // ← new field
}

export interface RentRequestDto {
  customerId: number;
  motorId: number;
  days: number;
}

export interface TransactionResponseDto {
  id: number;
  customerId: number;
  customerEmail: string;
  motorId: number;
  motorBrand: string;
  daysRented: number;
  totalAmount: number;
  dateRented: string;
}