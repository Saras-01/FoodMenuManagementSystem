import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject, tap } from 'rxjs';
import { FoodItem } from '../models/food-item';

@Injectable({
  providedIn: 'root'
})
export class FoodItemService {

  private apiUrl = 'https://localhost:7136/api/FoodItems';

  private foodItemsSubject = new BehaviorSubject<FoodItem[]>([]);

  foodItems$ = this.foodItemsSubject.asObservable();

  constructor(private http: HttpClient) {}

  getFoodItems(): Observable<FoodItem[]> {
    return this.http.get<FoodItem[]>(this.apiUrl).pipe(
      tap(items => this.foodItemsSubject.next(items))
    );
  }

  getFoodItemById(id: number): Observable<FoodItem> {
    return this.http.get<FoodItem>(`${this.apiUrl}/${id}`);
  }

  addFoodItem(foodItem: Omit<FoodItem, 'id'>): Observable<FoodItem> {
    return this.http.post<FoodItem>(this.apiUrl, foodItem).pipe(
      tap(createdItem => {
        this.foodItemsSubject.next([
          ...this.foodItemsSubject.value,
          createdItem
        ]);
      })
    );
  }

  updateFoodItem(
    id: number,
    foodItem: Omit<FoodItem, 'id'>
  ): Observable<FoodItem> {
    return this.http.put<FoodItem>(
      `${this.apiUrl}/${id}`,
      foodItem
    ).pipe(
      tap(updatedItem => {
        this.foodItemsSubject.next(
          this.foodItemsSubject.value.map(item =>
            item.id === id ? updatedItem : item
          )
        );
      })
    );
  }

  deleteFoodItem(id: number): Observable<void> {
    return this.http.delete<void>(
      `${this.apiUrl}/${id}`
    ).pipe(
      tap(() => {
        this.foodItemsSubject.next(
          this.foodItemsSubject.value.filter(item => item.id !== id)
        );
      })
    );
  }
}