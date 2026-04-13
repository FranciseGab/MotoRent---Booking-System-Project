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