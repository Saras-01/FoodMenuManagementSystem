import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule
} from '@angular/forms';

import { Subscription } from 'rxjs';

import { FoodItem } from '../../models/food-item';
import { FoodItemService } from '../../services/food-item.service';

@Component({
  selector: 'app-admin-dashboard',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './admin-dashboard.html',
  styleUrl: './admin-dashboard.css',
})
export class AdminDashboard implements OnInit, OnDestroy {

  foodForm: FormGroup;
  foodItems: FoodItem[] = [];

  isSaving = false;
  isLoading = false;
  isEditMode = false;

  editingFoodId: number | null = null;

  successMessage = '';
  errorMessage = '';

  private foodItemsSubscription?: Subscription;

  constructor(
    private fb: FormBuilder,
    private foodItemService: FoodItemService
  ) {
    this.foodForm = this.fb.group({
      name: ['', Validators.required],
      category: ['', Validators.required],
      description: [''],
      price: ['', [Validators.required, Validators.min(0.01)]],
      isAvailable: [true]
    });
  }

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
        console.error('Loading error:', error);
        this.errorMessage = 'Unable to load food items.';
        this.isLoading = false;
      }
    });
  }

  addFoodItem(): void {
    if (this.foodForm.invalid) {
      this.foodForm.markAllAsTouched();
      return;
    }

    this.isSaving = true;
    this.successMessage = '';
    this.errorMessage = '';

    const foodItem = this.getFormData();

    this.foodItemService.addFoodItem(foodItem).subscribe({
      next: (createdItem) => {
        this.successMessage =
          `${createdItem.name} added successfully!`;

        this.resetForm();
      },
      error: (error) => {
        console.error('Adding error:', error);

        this.errorMessage = error.status === 409
          ? 'A food item with this name already exists.'
          : 'Unable to add food item.';

        this.isSaving = false;
      }
    });
  }

  editFoodItem(item: FoodItem): void {
    this.isEditMode = true;
    this.editingFoodId = item.id;

    this.foodForm.patchValue({
      name: item.name,
      category: item.category,
      description: item.description ?? '',
      price: item.price,
      isAvailable: item.isAvailable
    });

    this.successMessage = '';
    this.errorMessage = '';
  }

  updateFoodItem(): void {
    if (this.foodForm.invalid || this.editingFoodId === null) {
      this.foodForm.markAllAsTouched();
      return;
    }

    this.isSaving = true;
    this.successMessage = '';
    this.errorMessage = '';

    const foodItem = this.getFormData();

    this.foodItemService.updateFoodItem(
      this.editingFoodId,
      foodItem
    ).subscribe({
      next: (updatedItem) => {
        this.successMessage =
          `${updatedItem.name} updated successfully!`;

        this.resetForm();
      },
      error: (error) => {
        console.error('Updating error:', error);

        this.errorMessage = error.status === 409
          ? 'Another food item already uses this name.'
          : 'Unable to update food item.';

        this.isSaving = false;
      }
    });
  }

  deleteFoodItem(id: number): void {
    const confirmed = window.confirm(
      'Are you sure you want to delete this food item?'
    );

    if (!confirmed) {
      return;
    }

    this.successMessage = '';
    this.errorMessage = '';

    this.foodItemService.deleteFoodItem(id).subscribe({
      next: () => {
        this.successMessage = 'Food item deleted successfully!';

        if (this.editingFoodId === id) {
          this.resetForm();
        }
      },
      error: (error) => {
        console.error('Deleting error:', error);
        this.errorMessage = 'Unable to delete food item.';
      }
    });
  }

  private getFormData(): Omit<FoodItem, 'id'> {
    const formData = this.foodForm.value;

    return {
      name: formData.name.trim(),
      category: formData.category.trim(),
      description: formData.description?.trim() || null,
      price: Number(formData.price),
      isAvailable: Boolean(formData.isAvailable)
    };
  }

  resetForm(): void {
    this.foodForm.reset({
      name: '',
      category: '',
      description: '',
      price: '',
      isAvailable: true
    });

    this.isEditMode = false;
    this.editingFoodId = null;
    this.isSaving = false;
  }

  submitForm(): void {
    if (this.isEditMode) {
      this.updateFoodItem();
    } else {
      this.addFoodItem();
    }
  }

  ngOnDestroy(): void {
    this.foodItemsSubscription?.unsubscribe();
  }
}