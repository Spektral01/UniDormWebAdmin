import { getAuth, signInWithEmailAndPassword } from "firebase/auth";
import { initializeApp } from "firebase/app";

const firebaseConfig = {
  apiKey: "AIzaSyAye7zgWp-lrmpW8fPKzVzmBo1C4vf00OY",
  authDomain: "unidormdb.firebaseapp.com",
  databaseURL:
    "https://unidormdb-default-rtdb.europe-west1.firebasedatabase.app",
  projectId: "unidormdb",
  storageBucket: "unidormdb.appspot.com",
  messagingSenderId: "860519113711",
  appId: "1:860519113711:web:140029b0fe2c12073e8c1a",
};

// Инициализация Firebase
const app = initializeApp(firebaseConfig);

// Получение объекта auth
const auth = getAuth(app);

const state = {
  user: null,
  error: null,
};

const mutations = {
  setUser(state, user) {
    state.user = user;
  },
  setError(state, error) {
    state.error = error;
  },
};

const actions = {
  async login({ commit }, { email, password }) {
    try {
      const userCredential = await signInWithEmailAndPassword(
        auth,
        email,
        password
      );
      commit("setUser", userCredential.user);
      commit("setError", null);
      return userCredential.user;
    } catch (error) {
      commit("setError", error.message);
      throw error;
    }
  },
};

const getters = {
  isAuthenticated(state) {
    return state.user !== null;
  },
};

export default {
  state,
  mutations,
  actions,
  getters,
};
