export interface IBooking {
    checkIn: string;
    checkOut: string;
    customerId: number;
    customerName: string;
    totalFee: number;
    hotelId: number;
    paymentTypeId: number;
    cardHolderNumber: string;
    cardNumber: string;
    expiryMM: string;
    expiryYY: string;
    cvv: number;
}
