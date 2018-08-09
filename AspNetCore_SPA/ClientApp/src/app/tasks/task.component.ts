import { Component, OnInit, Injectable } from "@angular/core";
import { Task, TaskService } from "./task.service";

@Component({
  selector: 'app-task',
  templateUrl: './task.component.html',
  styleUrls: ['./task.component.css']
})

export class TaskComponent implements OnInit {
  tasks: Array<Task> = [];
  errors: Array<string> = [];
  currentTask: Task = new Task();

  public editing: boolean = false;
  public showCompleted: boolean = false;

  constructor(private taskService: TaskService) { }

  async ngOnInit() {
    await this.getAll();
  }

  async toggleCompleted() {
    if (!this.showCompleted) {
      await this.getAll();
    } else {
      await this.getAllCompleted();
    }
  }

  async getAllCompleted() {
    await this.taskService
      .getAllCompleted()
      .subscribe(tasks => {
        this.tasks = tasks;
        this.showCompleted = false;
      });
  }

  async getAll() {
    await this.taskService
      .getAll()
      .subscribe(tasks => {
        this.tasks = tasks;
        this.showCompleted = true;
      });
  }

  async upsertTask() {
    this.errors.length = 0;

    await this.taskService
      .upsert(this.currentTask)
      .subscribe(task => {
        if (!this.currentTask.id) {
          this.tasks.push(task);
        }

        this.currentTask = new Task();
        this.editing = false;
      },
        error => this.parseErrors(error));
  }

  async deleteTask(task: Task) {
    if (window.confirm('Are sure you want to delete this item ?')) {
      await this.taskService.delete(task.id).subscribe(response => {
        var idx = this.tasks.indexOf(task);
        this.tasks.splice(idx, 1);
      });
    }
  }

  selectTask(task: Task) {
    if (this.currentTask == task) {
      this.currentTask = new Task();
      this.editing = false;
      this.errors.length = 0;
    } else {
      this.currentTask = task;
      this.editing = true;
    }
  }

  newTask() {
    if (this.currentTask.id != undefined) {
      this.editing = true;
    }
    else {
      this.editing = !this.editing;
    }
    this.currentTask = new Task();

    if (!this.editing) {
      this.errors.length = 0;
    }
  }

  // Processes validation error messages from ASP.NET Core Model Validation.
  // Writing second validation in Angular is not necessary then.
  private parseErrors(responseError) {
    for (var key in responseError.error) {
      for (var i = 0; i < responseError.error[key].length; i++) {
        this.errors.push(responseError.error[key][i]);
      }
    }
  }
}
