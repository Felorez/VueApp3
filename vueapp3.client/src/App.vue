<template>
  <div class="wrapper">
    <div class="conteiner">
      <h1 class="header">ToDo List</h1>
      <input type="text"
             class="text-input-add"
             placeholder="Add a new user..."
             v-model="newTaskName" />
      <input type="text"
             class="text-input-add"
             placeholder="User description..."
             v-model="newTaskDescription"
             @keyup.enter="addTask()" />
      <ul class="tasks-view">
        <div class="container-task-view"
             v-for="(task, index) in tasks"
             :key="task.userId">
          <p class="task-index">{{ index + 1 }}.</p>
        <li class="task-view">{{ task.userName }}: {{ task.userDescription }}</li>
        <p class="task-delete" @click="deleteTask(task.userId)">-</p>
    </div>
    </ul>
  </div>
  </div>
</template>

<script>
  import { ref, onMounted } from "vue";
  import axios from "axios";

  const API_BASE_URL = "http://localhost:5082/api/MyData";

  export default {
    name: "HomePage",
    setup() {
      const newTaskName = ref("");
      const newTaskDescription = ref("");
      const tasks = ref([]);

      // Получение задач
      const fetchTasks = async () => {
        try {
          const response = await axios.get(API_BASE_URL);
          tasks.value = response.data;
          console.log(tasks);
        } catch (error) {
          console.error("Error fetching tasks:", error);
        }
      };

      // Добавление задачи
      const addTask = async () => {
        if (!newTaskName.value || !newTaskDescription.value) return;

        try {
          const response = await axios.post(API_BASE_URL, {
            UserName: newTaskName.value,
            UserDescription: newTaskDescription.value,
          });
          tasks.value.unshift(response.data); // Добавляем новую задачу в список
          newTaskName.value = "";
          newTaskDescription.value = "";
        } catch (error) {
          console.error("Error adding task:", error);
        }
      };

      // Удаление задачи
      const deleteTask = async (id) => {
        try {
          await axios.delete(`${API_BASE_URL}/${id}`);
          tasks.value = tasks.value.filter((task) => task.userId !== id); // Удаляем из списка
        } catch (error) {
          console.error("Error deleting task:", error);
        }
      };

      // Загружаем задачи при монтировании
      onMounted(fetchTasks);

      return {
        newTaskName,
        newTaskDescription,
        tasks,
        addTask,
        deleteTask,
      };
    },
  };
</script>

<style scoped>
  /* Стили остаются прежними */
  .wrapper {
    width: 33.3%;
    height: 100%;
    position: relative;
    margin: 10px auto;
    padding: 1rem;
    background-color: rgba(255, 255, 255, 0.9);
    border-radius: 10px;
  }

  .header {
    text-align: center;
    margin: 1rem;
  }

  .text-input-add {
    transition: all 1s;
    min-width: 100%;
    min-height: 3svh;
    border: none;
    border-radius: 5px;
    margin-bottom: 10px;
  }

    .text-input-add:focus {
      outline: none;
      min-height: 4svh;
      animation: 1s ease-in-out slide-in;
    }

  .tasks-view {
    padding-left: 1px;
  }

  .container-task-view {
    display: flex;
    justify-content: space-between;
    margin-top: 1rem;
    border: 2.5px solid rgb(80, 80, 80);
    border-radius: 10px;
    padding: 5px;
    background-color: white;
    max-height: 2vw;
    align-items: center;
  }

  .task-index {
    margin-right: 10px;
  }

  .task-view {
    overflow: hidden;
    white-space: nowrap;
    text-overflow: ellipsis;
    text-align: center;
  }

  .task-delete {
    background-color: red;
    padding-left: 10px;
    padding-right: 10px;
    border-radius: 10px;
  }

  @keyframes slide-in {
    from {
      min-height: 3svh;
    }

    to {
      min-height: 4svh;
    }
  }
</style>
