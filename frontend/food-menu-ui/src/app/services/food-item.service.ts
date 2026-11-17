import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject, tap } from 'rxjs';

import { FoodItem } from '../models/food-item';

@Injectable({
  providedIn: 'root'
})
export class FoodItemService {

  private apiUrl = 'https://localhost:7136/api/fooditems';

  private foodItemsSubject = new BehaviorSubject<FoodItem[]>([]);

  foodItems$ = this.foodItemsSubject.asObservable();

  constructor(private http: HttpClient) {}

  // Get all food items from backend
  getFoodItems(): Observable<FoodItem[]> {
    return this.http.get<FoodItem[]>(this.apiUrl).pipe(
      tap(items => this.foodItemsSubject.next(items))
    );
  }

  // Get food item by ID
  getFoodItemById(id: number): Observable<FoodItem> {
    return this.http.get<FoodItem>(`${this.apiUrl}/${id}`);
  }

  // Add food item
  addFoodItem(foodItem: Omit<FoodItem, 'id'>): Observable<FoodItem> {
    return this.http.post<FoodItem>(this.apiUrl, foodItem).pipe(
      tap(createdItem => {
        const currentItems = this.foodItemsSubject.value;

        this.foodItemsSubject.next([
          ...currentItems,
          createdItem
        ]);
      })
    );
  }

  // Update food item
  updateFoodItem(
    id: number,
    foodItem: Omit<FoodItem, 'id'>
  ): Observable<FoodItem> {

    return this.http.put<FoodItem>(
      `${this.apiUrl}/${id}`,
      foodItem
    ).pipe(
      tap(updatedItem => {
        const currentItems = this.foodItemsSubject.value;

        this.foodItemsSubject.next(
          currentItems.map(item =>
            item.id === id ? updatedItem : item
          )
        );
      })
    );
  }

  // Delete food item
  deleteFoodItem(id: number): Observable<void> {

    return this.http.delete<void>(
      `${this.apiUrl}/${id}`
    ).pipe(
      tap(() => {
        const currentItems = this.foodItemsSubject.value;

        this.foodItemsSubject.next(
          currentItems.filter(item => item.id !== id)
        );
      })
    );
  }

}