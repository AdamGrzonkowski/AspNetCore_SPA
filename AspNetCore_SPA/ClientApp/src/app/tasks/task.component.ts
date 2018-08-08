import { Component, OnInit, Injectable } from "@angular/core";
import { Task, TaskService } from "./task.service";

@Component({
  selector: 'app-task',
  templateUrl: './task.component.html',
  styleUrls: ['./task.component.css']
})

export class TaskComponent implements OnInit {
  tasks: Array<Task> = [];
  public editing: boolean = false;
  currentTask: Task = new Task();

  constructor(private taskService: TaskService) { }

  async ngOnInit() {
    await this.taskService
      .getAll()
      .subscribe(tasks => (this.tasks = tasks));
  }

  async upsertTask() {
    await this.taskService
      .upsert(this.currentTask)
      .subscribe(task => {
        if (!this.currentTask.id) {
          this.tasks.push(task);
        }

        this.currentTask = new Task();
        this.editing = false;
      });
  }

  async deleteTask(task: Task) {
    await this.taskService.delete(task.id).subscribe(response => {
      var idx = this.tasks.indexOf(task);
      this.tasks.splice(idx, 1);
    });
  }

  selectTask(task: Task) {
    if (this.currentTask == task) {
      this.currentTask = new Task();
      this.editing = false;
    } else {
      this.currentTask = task;
      this.editing = true;
    }
  }

  newTask() {
    this.currentTask = new Task();
    this.editing = !this.editing;
  }
}
