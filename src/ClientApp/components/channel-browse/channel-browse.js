import ChannelJoin from "../../mixins/ChannelJoin";
import ChannelLeave from "../../mixins/ChannelLeave";
import DateTimeFormatter from "../../mixins/DatetimeFormatter";
import {EventTypes} from "../../stores/EventStore";
import DetailsPreloader from "../../mixins/DetailsPreloader";
import HttpErrorHandler from "../../mixins/HttpErrorHandler";

export default {
    mixins: [ChannelLeave, ChannelJoin, DateTimeFormatter, DetailsPreloader, HttpErrorHandler],
    props: ['channels', 'connectedChannels'],
    data: function () {
        return {
            notConnectedChannels: [],
            name: "",
            errors: []
        };
    },
    mounted: function () {
        this.reloadNotConnectedChannels();
        
        this.$events.on('AsyncDataReloaded', () => this.reloadNotConnectedChannels());
        
        this.$events.on(`SocketEventType:${EventTypes.ChannelCreated}`,(event) => {
            if (event.groupId === this.$parent.activeGroup.id) {
                this.reloadNotConnectedChannels();
            }
        });
        
        this.$events.on(`SocketEventType:${EventTypes.ChannelRemoved}`, (event) => {
            if (event.groupId === this.$parent.activeGroup.id) {
                this.reloadNotConnectedChannels();
            }
        });
    },
    computed: {
        classWhenError: function () {
            return {
                error: this.errors.length > 0,
                focused: this.errors.length > 0,
            }
        }
    },
    methods: {
        reloadNotConnectedChannels: function () {
            if (this.$parent.isActiveGroupNotEmpty()) {
                this.showDetailsPreloader();
                
                this.$http.get("/api/group/" + this.$parent.activeGroup.id + "/channel/notConnected").then(
                    (response) => { 
                        this.notConnectedChannels = response.body; 
                        this.hideDetailsPreloader();
                    },
                    (error) => {
                        this.hideDetailsPreloader();
                        this.handleHttpNotValidationErrors(error);
                    }
                );
            }
        },
        isChannelConnected: function(channel) {
            let filtered = this.connectedChannels.filter(function (connectedChannel) {
                return channel.id === connectedChannel.id;
            });
            
            return filtered.length > 0;
        }
    }
}