import { Component, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Subscription } from 'rxjs';

import { FoodItem } from '../../models/food-item';
import { FoodItemService } from '../../services/food-item.service';

@Component({
  selector: 'app-dashboard',
  imports: [CommonModule],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css'
})
export class Dashboard implements OnInit, OnDestroy {

  foodItems: FoodItem[] = [];

  isLoading = true;
  errorMessage = '';

  private foodItemsSubscription?: Subscription;

  constructor(
    private foodItemService: FoodItemService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.subscribeToFoodItems();
    this.loadDashboardData();
  }

  loadDashboardData(): void {

    this.isLoading = true;
    this.errorMessage = '';

    this.cdr.detectChanges();

    this.foodItemService.getFoodItems().subscribe({

      next: (items) => {

        this.foodItems = items;
        this.isLoading = false;

        this.cdr.detectChanges();

        console.log('Dashboard loaded successfully:', items);

      },

      error: (error) => {

        console.error('Dashboard loading error:', error);

        this.errorMessage =
          'Unable to load dashboard data. Please try again.';

        this.isLoading = false;

        this.cdr.detectChanges();

      }

    });

  }

  subscribeToFoodItems(): void {

    this.foodItemsSubscription =
      this.foodItemService.foodItems$.subscribe(items => {

        this.foodItems = items;

        this.cdr.detectChanges();

      });

  }

  get totalItems(): number {
    return this.foodItems.length;
  }

  get availableItems(): number {
    return this.foodItems.filter(
      item => item.isAvailable
    ).length;
  }

  get unavailableItems(): number {
    return this.foodItems.filter(
      item => !item.isAvailable
    ).length;
  }

  get totalCategories(): number {
    return new Set(
      this.foodItems.map(item => item.category)
    ).size;
  }

  ngOnDestroy(): void {
    this.foodItemsSubscription?.unsubscribe();
  }

}