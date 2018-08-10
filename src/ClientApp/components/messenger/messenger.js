import SocketManager, { SocketTypes } from "../../services/socket/SocketManager";
import ScrollDown from "../../mixins/ScrollDown";
import HttpErrorHandler from "../../mixins/HttpErrorHandler";
import DetailsPreloader from "../../mixins/DetailsPreloader";
import ChannelLeave from "../../mixins/ChannelLeave";
import DatetimeFormatter from "../../mixins/DatetimeFormatter";

String.prototype.capitalize = function() {
    return this.charAt(0).toUpperCase() + this.slice(1);
};

export default {
    mixins: [ScrollDown, HttpErrorHandler, DetailsPreloader, ChannelLeave, DatetimeFormatter],
    props: ['account'],
    data: function () {
        return {
            message: null,
            messages: [],
            type: null
        }
    },
    created: function () {
        this.init();
    },
    mounted: function () {
        this.invokeUnreadedCountersReset();
    },
    watch: {
        '$route.params.id': function () {
            this.dispose();
            this.init();
            this.invokeUnreadedCountersReset();
        }
    },
    beforeRouteLeave: function (to, from, next) {
        this.dispose();
        next();
    },
    beforeDestroy: function () {
        this.dispose();
    },
    computed: {
        sortedMessages: function () {
            return this.messages.sort((one, another) => new Date(one.created) > new Date(another.created));
        },
        isOwnedMessage: function () {
            return (userId) => {
                return this.account.id === userId;
            }
        }
    },
    methods: {
        sendMessage: function () {
            if (this.message == null || this.message.trim().length <= 0) {
                return;
            }

            let message = { content: this.message };
            message[this.type + "Id"] = this.$route.params.id;

            let serializedMessage = JSON.stringify(message);
            SocketManager.sendMessage(SocketTypes.SendMessage, serializedMessage);

            this.message = null;
        },
        addMessage: function (message) {
            if (this.messages.filter(m => m.id === message.id).length === 0) {
                this.messages.push(message);
                this.scrollDownOnNextUpdate();
            }
        },
        init: function () {
            this.type = this.$route.matched.filter(record => !!record.meta.type).pop().meta.type;
            
            if (!this.type) {
                throw "type must be defined in 'meta' before init";
            }

            this.fetchMessages();

            if (SocketManager.isConnected(SocketTypes.ReceiveMessage)) {
                SocketManager.addMessageListener(SocketTypes.ReceiveMessage, this.receiveMessageListener);
            } else {
                this.$events.on('socketsConnected', () => {
                    SocketManager.addMessageListener(SocketTypes.ReceiveMessage, this.receiveMessageListener);
                });
            }
        },
        dispose: function () {
            this.messages = [];
            SocketManager.removeMessageListener(SocketTypes.ReceiveMessage, this.receiveMessageListener);
        },
        receiveMessageListener: function (message) {
            let data = JSON.parse(message.data);

            if (
                (data.chatId == this.$route.params.id && this.type == "chat") ||
                (data.channelId == this.$route.params.id && this.type == "channel")
            ) {
                this.addMessage(data);
            }
        },
        fetchMessages: function () {
            this.showDetailsPreloader();
            
            this.$http.get("api/" + this.type + "/" + this.$route.params.id + "/message").then(
                (response) => {
                    for (let message of response.body) {
                        this.addMessage(message);
                    }
                    this.hideDetailsPreloader();
                },
                (error) => {
                    this.hideDetailsPreloader();
                    this.handleHttpNotValidationErrors(error);
                }
            );
        },
        invokeUnreadedCountersReset: function () {
            this.$events.emit(`Reset${this.type.capitalize()}Counter`, {
                id: this.$route.params.id
            });
        },
        leaveThisChannel: function () {
            this.leaveChannelById(this.$route.params.id);
        }
    }
}