<template>
  <q-layout view="hHh lpR fFf">
    <q-page-container>
      <div class="login-page">
        <div class="login-form q-gutter-md">
          <div class="text-h6 text-center">Авторизация</div>
          <q-input filled v-model="email" label="Email" type="email" />
          <q-input filled v-model="password" label="Password" type="password" />
          <q-btn
            class="full-width"
            color="primary"
            label="Войти"
            @click="handleLogin"
          />
        </div>
      </div>
    </q-page-container>
    <q-dialog v-model="errorDialogVisible" persistent>
      <q-card>
        <q-card-section>
          <p>Вы ввели неверный логин или пароль.</p>
        </q-card-section>
        <q-card-actions align="right">
          <q-btn
            label="OK"
            color="primary"
            @click="errorDialogVisible = false"
          />
        </q-card-actions>
      </q-card>
    </q-dialog>
  </q-layout>
</template>

<script>
import { mapActions } from "vuex";

export default {
  data() {
    return {
      email: "",
      password: "",
      errorDialogVisible: false,
    };
  },
  methods: {
    ...mapActions(["login"]),
    async handleLogin() {
      try {
        await this.login({ email: this.email, password: this.password });
        this.$router.push("/notification");
      } catch (error) {
        this.errorDialogVisible = true;
      }
    },
  },
};
</script>

<style scoped>
.login-page {
  display: flex;
  justify-content: center;
  align-items: center;
  height: 100vh;
}

.login-form {
  max-width: 300px;
  width: 100%;
}

.full-width {
  width: 100%;
}
</style>
