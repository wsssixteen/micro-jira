import { Component, signal, inject, PLATFORM_ID } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TaskService, TaskItem } from './services/task.service';
import { isPlatformBrowser } from '@angular/common';
// added due to reset
import { switchMap } from 'rxjs/operators'; 

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, CommonModule, FormsModule],
  templateUrl: './app.html',
  styleUrl: './app.css',
  standalone: true
})
export class App {
  protected readonly title = signal('task-manager-ui');

  // UI state
  tasks = signal<TaskItem[]>([]);
  newTitle = signal('');
  editingId = signal<number | null>(null);
  editTitle = signal('');
  isDarkMode = signal(false);
  
  // Profile state
  showProfile = signal(false);
  profilePhoto = signal('/photo.jpg');
  profileName = signal('Ahmad Ridhwan Anuar');
  profileTitle = signal('Systems Engineer');
  profileBio = signal('Technology enthusiast that loves implementing efficient solutions and continuous learning.');

  // UI toggles
  // collapsed by default for portfolio presentation
  showCompleted = signal(false);

  private platformId = inject(PLATFORM_ID);

  // convenience getters for view
  get activeTasks() {
    return this.tasks().filter(t => !t.isComplete);
  }

  get completedTasks() {
    return this.tasks().filter(t => t.isComplete);
  }

  toggleCompleted(): void {
    this.showCompleted.update(v => !v);
  }

  constructor(private taskService: TaskService) {
    console.log('TRACE 1: Component initialized. Starting loadTasks().');
    // check dark mode preference
    this.loadTasks();
    // Optional: Check local storage for user's preference on startup
    if (isPlatformBrowser(this.platformId)) {
      const savedMode = localStorage.getItem('theme');
      if (savedMode === 'dark') {
        this.isDarkMode.set(true);
      }
    }
  }

  toggleDarkMode(): void {
    this.isDarkMode.update(value => {
      const newMode = !value;
      if (isPlatformBrowser(this.platformId)) {
        localStorage.setItem('theme', newMode ? 'dark' : 'light');
      }
      return newMode;
    });
  }

  loadTasks(): void {
    this.taskService.getTasks().subscribe({
      next: (items) => {
        console.log('LOAD TASKS FROM API:', items);
        this.tasks.set(items);
      },
      error: (err) => console.error('Failed to load tasks', err)
    });
  }

  addTask(): void {
    const title = this.newTitle().trim();
    if (!title) return;

    this.taskService.addTask({ title }).subscribe({
      next: (created) => {
        // append to local list
        this.tasks.update(current => [...current, created]);
        this.newTitle.set('');
      },
      error: (err) => console.error('Failed to add task', err)
    });
  }

  deleteTask(id: number | undefined): void {
    if (!id) return;
    this.taskService.deleteTask(id).subscribe({
      next: () => {
        this.tasks.update(current => current.filter(task => task.id !== id));
      },
      error: (err) => console.error('Failed to delete task', err)
    });
  }

  confirmDelete(id: number | undefined): void {
    if (!id) return;
    if (window.confirm('Are you sure you want to delete this task?')) {
      this.deleteTask(id);
    }
  }

  completeTask(id: number | undefined): void {
    if (!id) return;
    this.taskService.toggleComplete(id).subscribe({
      next: (updated) => {
        this.tasks.update(current => current.map(t => t.id === updated.id ? { ...t, isComplete: updated.isComplete } : t));
      },
      error: (err) => console.error('Failed to toggle complete', err)
    });
  }

  startEdit(task: TaskItem): void {
    this.editingId.set(task.id || null);
    this.editTitle.set(task.title);
  }

  cancelEdit(): void {
    this.editingId.set(null);
    this.editTitle.set('');
  }

  saveEdit(id: number | undefined): void {
    if (!id || !this.editTitle().trim()) return;
    const updatedTask = { id, title: this.editTitle() };
    this.taskService.updateTask(id, updatedTask).subscribe({
      next: () => {
        this.tasks.update(current =>
          current.map(t => (t.id === id ? { ...t, title: this.editTitle() } : t))
        );
        this.editingId.set(null);
        this.editTitle.set('');
      },
      error: (err) => console.error('Failed to update task', err)
    });
  }

  resetTasks(): void {
    if (window.confirm('This will reset the tasks in the page. Continue?')) {
      this.taskService.resetDemoData().pipe(
        switchMap(() => {
          console.log('Tasks successfully reset on the backend. Fetching reset lists...');
          return this.taskService.getTasks();
        })
      ).subscribe({
        next: (items) => {
          console.log('Tasks successfully reset.');
          this.tasks.set(items); 
        },
        error: (err) => console.error('Failed to reset tasks', err)
      });
    }
  }

  toggleProfile(): void {
    this.showProfile.update(current => !current);
  }
}