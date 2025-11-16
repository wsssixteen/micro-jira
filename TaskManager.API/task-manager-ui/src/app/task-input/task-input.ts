import { Component, OnInit, OnDestroy, ElementRef, ViewChild } from '@angular/core';

@Component({
  selector: 'app-task-input',
  templateUrl: './task-input.component.html'})
export class TaskInputComponent implements OnInit, OnDestroy {
  
  // List of phrases to cycle through
  placeholders: string[] = [
    "e.g., Call the doctor",
    "e.g., Finish CSS Refactor", 
    "e.g., Buy groceries"
  ];
  
  // Property bound to the input's placeholder attribute
  currentPlaceholder: string = '';
  
  // Animation control variables
  private phraseIndex: number = 0;
  private charIndex: number = 0;
  private isDeleting: boolean = false;
  private typingSpeed: number = 100;
  private typingTimer: any;

  @ViewChild('taskInput') taskInput!: ElementRef<HTMLInputElement>;
  
  // We use OnInit to start the animation when the component loads
  ngOnInit(): void {
    this.startTypingAnimation();
  }

  // We use OnDestroy to clear the timer when the component is removed
  ngOnDestroy(): void {
    this.stopTypingAnimation();
  }

  /**
   * Starts the animation loop using setInterval.
   */
  startTypingAnimation(): void {
    if (this.typingTimer) {
        clearInterval(this.typingTimer);
    }
    this.typingTimer = setInterval(() => this.typeWriter(), this.typingSpeed);
  }

  /**
   * Clears the interval to stop the animation.
   */
  stopTypingAnimation(): void {
    clearInterval(this.typingTimer);
  }

  /**
   * The core logic for typing forward and deleting backward.
   */
  private typeWriter(): void {
    const currentPhrase = this.placeholders[this.phraseIndex];

    if (!this.isDeleting) {
      // 1. TYPING FORWARD
      this.currentPlaceholder = currentPhrase.substring(0, this.charIndex + 1);
      this.charIndex++;
      
      if (this.charIndex === currentPhrase.length) {
        this.isDeleting = true;
        this.pauseAndRestart(2000); // Pause for 2 seconds
      }
    } else {
      // 2. DELETING BACKWARD
      this.currentPlaceholder = currentPhrase.substring(0, this.charIndex - 1);
      this.charIndex--;
      
      if (this.charIndex === 0) {
        this.isDeleting = false;
        this.phraseIndex = (this.phraseIndex + 1) % this.placeholders.length; // Move to next phrase
        this.pauseAndRestart(500); // Short pause before typing next phrase
      }
    }
  }

  /**
   * Pauses the current animation loop and sets up a new one with a delay.
   * @param delayMs The time to pause in milliseconds.
   */
  private pauseAndRestart(delayMs: number): void {
      this.stopTypingAnimation();
      // Restart the loop with the specified delay
      this.typingTimer = setTimeout(() => {
          this.typingTimer = setInterval(() => this.typeWriter(), this.typingSpeed);
      }, delayMs);
  }

  /**
   * Event handler when the user focuses the input field.
   */
  onFocus(): void {
    this.stopTypingAnimation();
    // Optional: Set a final static placeholder when active
    this.currentPlaceholder = "Enter your new task..."; 
  }

  /**
   * Event handler when the user blurs the input field.
   */
  onBlur(event: Event): void {
    const input = event.target as HTMLInputElement;
    // Resume animation only if the field is empty
    if (input.value.trim() === '') {
      this.charIndex = 0; // Reset index to start typing from scratch
      this.isDeleting = false;
      this.phraseIndex = (this.phraseIndex + 1) % this.placeholders.length; // Start with the next phrase
      this.startTypingAnimation();
    }
  }

}