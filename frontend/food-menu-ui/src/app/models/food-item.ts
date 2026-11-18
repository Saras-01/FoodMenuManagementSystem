export interface FoodItem {
  id: number;
  name: string;
  category: string;
  description: string | null;
  price: number;
  isAvailable: boolean;
  imageUrl?: string;
}