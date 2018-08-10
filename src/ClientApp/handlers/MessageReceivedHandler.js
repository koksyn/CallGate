export default {
    data: function () {
        return {
            unreadedChatMessageCounter: {},
            unreadedChannelMessageCounter: {}
        }
    },
    mounted: function () {
        this.$events.on('MessageReceived', (message) => this.handleMessage(message));
        this.$events.on('ResetChannelCounter', (data) => this.resetChannelCounter(data.id));
        this.$events.on('ResetChatCounter', (data) => this.resetChatCounter(data.id));
    },
    methods: {
        handleMessage: function (message) {
            if(message.userId !== this.account.id) {
                this.handleMessageFromOtherUser(message);
            }
        },
        handleMessageFromOtherUser: function (message) {
            if (message.hasOwnProperty('chatId')) {
                this.handleChatMessageFromOtherUser(message);
            } else if (message.hasOwnProperty('channelId')) {
                this.handleConnectedChannelMessageFromOtherUser(message);
            }
        },
        handleChatMessageFromOtherUser: function (message) {
            let isNotChatMessengerOpened = (this.$route.fullPath !== `/chat/${message.chatId}`);
            
            if (isNotChatMessengerOpened) {
                let connectedChatFound = this.connectedChats.some((connectedChat) => (connectedChat.id === message.chatId));
                
                if (connectedChatFound) {
                    this.rippleIncomingUnreadedMessageButton(message.chatId);
                    this.increaseUnreadedCounterForChatId(message.chatId);
                }
            }
        },
        handleConnectedChannelMessageFromOtherUser: function (message) {
            let isNotChannelMessengerOpened = (this.$route.fullPath !== `/channel/${message.channelId}`);

            if (isNotChannelMessengerOpened) {
                let connectedChannelFound = this.connectedChannels.some((connectedChannel) => (connectedChannel.id === message.channelId));

                if (connectedChannelFound) {
                    this.rippleIncomingUnreadedMessageButton(message.channelId);
                    this.increaseUnreadedCounterForChannelId(message.channelId);
                }
            }
        },
        increaseUnreadedCounterForChatId: function (chatId) {
            let counterForChatExist = this.unreadedChatMessageCounter.hasOwnProperty(chatId);
            
            if (counterForChatExist) {
                let incremented = this.unreadedChatMessageCounter[chatId] + 1;
                this.$set(this.unreadedChatMessageCounter, chatId, incremented);
            } else {
                this.$set(this.unreadedChatMessageCounter, chatId, 1);
            }
        },
        increaseUnreadedCounterForChannelId: function (channelId) {
            let counterForChannelExist = this.unreadedChannelMessageCounter.hasOwnProperty(channelId);
            
            if (counterForChannelExist) {
                let incremented = this.unreadedChannelMessageCounter[channelId] + 1;
                this.$set(this.unreadedChannelMessageCounter, channelId, incremented);
            } else {
                this.$set(this.unreadedChannelMessageCounter, channelId, 1);
            }
        },
        resetChannelCounter: function (channelId) {
            this.$set(this.unreadedChannelMessageCounter, channelId, 0);
        },
        resetChatCounter: function (chatId) {
            this.$set(this.unreadedChatMessageCounter, chatId, 0);
        }
    }
}