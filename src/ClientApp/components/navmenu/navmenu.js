import MessageReceivedHandler from "../../handlers/MessageReceivedHandler";

export default {
    mixins: [MessageReceivedHandler],
    props: [
        'users', 
        'usersOutsidePrivateChats',
        'connectedChats', 
        'channels', 
        'connectedChannels', 
        'account'
    ],
    methods: {
        unreadedChat: function (chatId) {            
            return (chatId in this.unreadedChatMessageCounter) && (this.unreadedChatMessageCounter[chatId] > 0);
        },
        unreadedChannel: function (channelId) {
            return (channelId in this.unreadedChannelMessageCounter) && (this.unreadedChannelMessageCounter[channelId] > 0);
        },
        getUsernamesFromUsers: function (users) {
            let usernames = [];

            users.forEach(function (user) {
                usernames.push(user.username);
            });

            return usernames.join(', ');
        },
        getUsersOutsideChats: function() {
            return this.$parent.usersOutsidePrivateChats;
        },
        rippleIncomingUnreadedMessageButton: function (buttonId) {
            let button = document.getElementById(buttonId);

            Waves.init({ duration: 550 });
            Waves.attach(button, ['waves-block']);
            
            setTimeout(() => {
                Waves.calm(button);
                $(button).removeClass('waves-block waves-effect')
            }, 550);
            
            Waves.ripple(button);
        }
    }
}