<template>
  <div class="content">
    <q-layout view="hHh lpR fFf">
      <q-page-container>
        <q-header class="bg-primary text-white" height-hint="98" fixed>
          <q-toolbar>
            <q-toolbar-title>
              <q-avatar>
                <img
                  src="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRqJhV0BsZQGBGfxK-8EHS05JF4vdeYvN204NHOLOMg4A&s"
                />
              </q-avatar>
              Студенты
            </q-toolbar-title>
          </q-toolbar>

          <q-tabs align="left">
            <q-route-tab to="/notification" label="Объявления" />
            <q-route-tab to="/shop" label="Магазин" />
            <q-route-tab to="/dormitory" label="Общежития" />
            <q-route-tab to="/user" label="Студенты" />
          </q-tabs>
        </q-header>

        <div
          style="
            display: flex;
            justify-content: center;
            margin-left: 2%;
            margin-top: 10px;
          "
        >
          <q-input
            v-model="searchName"
            filled
            bg-color="white"
            label="Поиск по имени"
            style="flex: 4"
          />
          <div style="flex: 1; display: flex; margin-right: 10%">
            <q-btn
              push
              color="primary"
              label="Найти"
              @click="searchUsersByName()"
              style="margin-left: 10px"
            />
            <q-btn
              push
              color="primary"
              label="Найти всех"
              @click="fetchUsers()"
              style="margin-left: 10px"
            />
          </div>

          <div class="add-container">
            <q-btn
              push
              color="primary"
              label="Добавить студента"
              @click="addUser()"
            />
          </div>
        </div>

        <div
          v-for="(user, index) in users"
          :key="index"
          class="input-container"
        >
          <div class="q-pa-md row-horizontal-alignment">
            <div class="row justify-between">
              <div class="input-fields">
                <q-input
                  v-model="user.dorm"
                  filled
                  bg-color="white"
                  label="Dorm"
                  class="q-mb-md"
                />
                <q-input
                  v-model="user.name"
                  filled
                  bg-color="white"
                  label="Name"
                  class="q-mb-md"
                />
                <q-input
                  v-model="user.phone"
                  filled
                  bg-color="white"
                  label="Phone"
                  class="q-mb-md"
                />
                <q-input
                  v-model="user.room"
                  filled
                  bg-color="white"
                  label="Room"
                  class="q-mb-md"
                />
                <q-input
                  v-model="user.email"
                  filled
                  bg-color="white"
                  label="Email"
                  class="q-mb-md"
                />
              </div>
            </div>
            <div class="q-mb-md" style="display: flex; justify-content: center">
              <q-btn
                @click="pushToServer(user)"
                push
                color="primary"
                label="Загрузить"
                style="width: 25%"
              />
            </div>
            <div class="q-mb-md" style="display: flex; justify-content: center">
              <q-btn
                @click="deleteUser(index, user)"
                push
                color="negative"
                label="Удалить"
                style="width: 25%"
              />
            </div>
          </div>
        </div>
      </q-page-container>
    </q-layout>
  </div>
</template>

<script>
import { ref, onMounted } from "vue";

export default {
  setup() {
    // Initialize an array to store users
    const users = ref([]);

    // Method to fetch users from the server
    const fetchUsers = () => {
      console.log("Fetching users...");
      fetch("http://localhost:5111/User/GetUsers")
        .then((response) => {
          console.log("Response status:", response.status);
          return response.json();
        })
        .then((data) => {
          console.log("Users data:", data);
          users.value = data;
        })
        .catch((error) => {
          console.error("Error fetching users:", error);
        });
    };

    // Method to delete a user
    const deleteUser = (index, user) => {
      users.value.splice(index, 1);

      const { userId } = user;

      fetch(`http://localhost:5111/User/DeleteUser?userId=${userId}`, {
        method: "DELETE",
        headers: {
          "Content-Type": "application/json",
        },
      })
        .then((response) => {
          if (!response.ok) {
            throw new Error(
              `Network response was not ok. Status: ${response.status}`
            );
          }
          return response.json();
        })
        .then((data) => {
          console.log("Success:", data);
          alert(data.message); // Show success message
        })
        .catch((error) => {
          console.error("Error:", error.message);
          alert("Failed to delete user. Please try again later.");
        });
    };

    // Method to push a user to the server
    const pushToServer = (user) => {
      fetch("http://localhost:5111/User/CreateUser", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(user),
      })
        .then((response) => {
          if (!response.ok) {
            throw new Error(
              `Network response was not ok. Status: ${response.status}`
            );
          }
          return response.json();
        })
        .then((data) => {
          console.log("Success:", data);
          alert(data.message); // Show success message
        })
        .catch((error) => {
          console.error("Error:", error.message);
          alert("Failed to add user. Please try again later.");
        });
    };

    const searchName = ref(""); // Инициализация переменной для хранения введенного имени

    // Метод для отправки запроса на сервер по введенному имени
    const searchUsersByName = () => {
      console.log("Searching users by name:", searchName.value); // Лог в консоль введенного имени
      fetch(
        `http://localhost:5111/User/GetUsersByName?name=${searchName.value}`
      )
        .then((response) => response.json())
        .then((data) => {
          console.log("Users data by name:", data);
          users.value = data;
        })
        .catch((error) => {
          console.error("Error fetching users by name:", error);
        });
    };

    // Method to add a new user
    const addUser = () => {
      const newUser = {
        dorm: "",
        name: "",
        phone: "",
        room: "",
        userId: "",
        email: "",
      };
      users.value = [];
      users.value.push(newUser);
    };

    return {
      users,
      searchName,
      deleteUser,
      pushToServer,
      addUser,
      fetchUsers,
      searchUsersByName,
    };
  },
};
</script>

<style>
.my-header {
  width: 100%;
  z-index: 1000;
}

.input-container {
  margin-top: 20px;
  margin-right: 20px;
  margin-left: 20px;
  background-color: lightgray;
  padding: 16px;
  border-radius: 16px;
}

.input-fields {
  flex: 7;
}

.add-container {
  margin-top: 20px;
  margin-right: 20px;
  margin-left: 20px;
  display: flex;
  justify-content: flex-end;
}
</style>
