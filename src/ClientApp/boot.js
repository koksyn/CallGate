import './css/site.css';
import Vue from 'vue';
import AuthenticationService from "./services/AuthenticationService";
import VueRouter from 'vue-router';
import VueResource from "vue-resource";

Vue.use(VueResource);
Vue.use(VueRouter);

AuthenticationService.setBearerToken();

const routes = [
    { path: '/', component: require('./components/home/home.vue.html') },
    { path: '/login', meta: { anonymous: true }, component: require('./components/login/login.vue.html')},
    { path: '/register', meta: { anonymous: true }, component: require('./components/register/register.vue.html')},
    { path: '/confirmRegistration', meta: { anonymous: true }, component: require('./components/register/confirmRegistration.vue.html')},
    { path: '/settings', meta: { allowedWithoutGroups: true }, component: require('./components/settings/settings.vue.html')},
    // group
    { path: '/group/create', meta: { allowedWithoutGroups: true }, component: require('./components/group-create/group-create.vue.html')},
    { path: '/group/edit', meta: { onlyForGroupAdmin: true }, component: require('./components/group-edit/group-edit.vue.html')},
    { path: '/group/user/add', meta: { onlyForGroupAdmin: true }, component: require('./components/group-user-add/group-user-add.vue.html')},
    { path: '/group/user/manage', meta: { onlyForGroupAdmin: true }, component: require('./components/group-user-manage/group-user-manage.vue.html')},
    // channel
    { path: '/channel/browse', component: require('./components/channel-browse/channel-browse.vue.html') },
    { path: '/channel/create', component: require('./components/channel-create/channel-create.vue.html') },
    { path: '/channel/:id/user/manage', component: require('./components/channel-user-manage/channel-user-manage.vue.html') },
    { path: '/channel/:id', meta: { type: "channel" }, component: require('./components/messenger/messenger.vue.html') },
    // chat
    { path: '/chat/create/:userId', component: require('./components/chat-create/chat-create.vue.html') },
    { path: '/chat/:id/user/manage', component: require('./components/chat-user-manage/chat-user-manage.vue.html') },
    { path: '/chat/:id', meta: { type: "chat" }, component: require('./components/messenger/messenger.vue.html') }
];

const router = new VueRouter({
    mode: 'history',
    routes: routes,
    linkActiveClass: "active", // active class for non-exact links.
    linkExactActiveClass: "active", // active class for *exact* links.
});

router.beforeEach((to, from, next) => {
    let anonymousRoute = to.matched.some(record => record.meta.anonymous);
    
    if (anonymousRoute) {
        AuthenticationService.isAuthenticated() ? next({ path: '/' }) : next();
    } else {
        AuthenticationService.isAuthenticated() ? next() : next({ path: '/login' });
    }
});

new Vue({
    el: '#app-root',
    router,
    render: h => h(require('./components/app/app.vue.html'))
});
