import { Component, OnInit, Injectable } from "@angular/core";
import { Task, TaskService } from "./task.service";

@Component({
  selector: 'app-task',
  templateUrl: './task.component.html'
})

export class TaskComponent implements OnInit {
  tasks: Array<Task> = [];
  currentTask: Task = new Task();

  constructor(private taskService: TaskService) { }

  async ngOnInit() {
    await this.taskService
      .getAll()
      .subscribe(tasks => (this.tasks = tasks));
  }

  async addTask() {
    await this.taskService
      .add(this.currentTask)
      .subscribe(task => {
        if (!this.currentTask.id) {
          this.tasks.push(task);
        }
        this.currentTask = new Task();
      });
  }

  async updateTask() {
    await this.taskService
      .update(this.currentTask)
      .subscribe(task => {
        if (!this.currentTask.id) {
          this.tasks.push(task);
        }
        this.currentTask = new Task();
      });
  }

  async deleteTask(task: Task) {
    await this.taskService.delete(task.id).subscribe(response => {
      var idx = this.tasks.indexOf(task);
      this.tasks.splice(idx, 1);
    });
  }

  selectTask(task: Task) {
    this.currentTask = task;
  }
}
