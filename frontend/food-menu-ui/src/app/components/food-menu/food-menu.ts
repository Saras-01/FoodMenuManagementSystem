import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Subscription } from 'rxjs';

import { FoodItem } from '../../models/food-item';
import { FoodItemService } from '../../services/food-item.service';

@Component({
  selector: 'app-food-menu',
  imports: [CommonModule, FormsModule],
  templateUrl: './food-menu.html',
  styleUrl: './food-menu.css',
})
export class FoodMenu implements OnInit, OnDestroy {

  foodItems: FoodItem[] = [];

  searchText = '';
  selectedCategory = 'All';

  isLoading = true;
  errorMessage = '';

  private foodItemsSubscription?: Subscription;

  constructor(
    private foodItemService: FoodItemService
  ) {}

  ngOnInit(): void {
    this.subscribeToFoodItems();
    this.loadFoodItems();
  }

  subscribeToFoodItems(): void {
    this.foodItemsSubscription =
      this.foodItemService.foodItems$.subscribe(items => {
        this.foodItems = items;
      });
  }

  loadFoodItems(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.foodItemService.getFoodItems().subscribe({
      next: () => {
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Food loading error:', error);

        this.errorMessage =
          'Unable to load the food menu. Please try again.';

        this.isLoading = false;
      }
    });
  }

  get categories(): string[] {
    return [
      'All',
      ...new Set(this.foodItems.map(item => item.category))
    ];
  }

  get filteredFoodItems(): FoodItem[] {
    return this.foodItems.filter(item => {

      const matchesCategory =
        this.selectedCategory === 'All' ||
        item.category === this.selectedCategory;

      const matchesSearch =
        item.name.toLowerCase().includes(
          this.searchText.toLowerCase()
        );

      return matchesCategory &&
        matchesSearch &&
        item.isAvailable;
    });
  }

  ngOnDestroy(): void {
    this.foodItemsSubscription?.unsubscribe();
  }
}