using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using AdaptiveCards;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;

namespace AdaptiveCard.Dialogs
{
    [LuisModel("fe4f5369-ee51-487c-ba81-bf1d4751ec5b", "f1aa63877da54b88af9fb32ca415bdca")]

    [Serializable]
    public class RootDialog : LuisDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {

            var activity = await result as Activity;
            
        }
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            try
            {

                await context.PostAsync("I didn't get you please ask service related queries!!!");

            }
            catch (Exception e)
            {

                throw;
            }
        }
        [LuisIntent("Services")]
        public async Task Services(IDialogContext context, LuisResult result)
        {

            var reply = context.MakeMessage();
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            AdaptiveCards.AdaptiveCard card2 = new AdaptiveCards.AdaptiveCard();
            await Greet(context, result);
            card2.Body.Add(new TextBlock()
            {
                Text = "Please Select:"
            });
            card2.Body.Add(new ChoiceSet()
            {
                Id = "test",
                Style = ChoiceInputStyle.Expanded,
                Choices = new List<Choice>() {
            new Choice() { Title="Service request",Value="Service request",IsSelected=true},
            new Choice() { Title="Incidence",Value="Incidence"},
            new Choice() { Title="Trouble Shooting",Value="Trouble Shooting"}
            }
            });

            card2.Actions.Add(new SubmitAction() { Title = "Submit" });

            Attachment attachment2 = new Attachment()
            {
                ContentType = AdaptiveCards.AdaptiveCard.ContentType,
                Content = card2
            };


            reply.Attachments.Add(attachment2);
            await context.PostAsync(reply);
            context.Wait(MessageReceivedAsync1);
        }

        private async Task MessageReceivedAsync1(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            var reply = context.MakeMessage();
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;

            if (activity.Value != null)
            {
                JObject res = JObject.Parse(activity.Value.ToString());
                string res1 = res.Property("test").Value.ToString();

                AdaptiveCards.AdaptiveCard card3 = new AdaptiveCards.AdaptiveCard();
                card3.Body.Add(new TextBlock()
                {
                    Text = $"Thank you for selecting ! {res1}"
                });
                Attachment attachment3 = new Attachment()
                {
                    ContentType = AdaptiveCards.AdaptiveCard.ContentType,
                    Content = card3
                };
                reply.Attachments.Add(attachment3);
                await context.PostAsync(reply);
                context.Done("");
            }
        }

        [LuisIntent("Greet")]
        public async Task Greet(IDialogContext context, LuisResult result)
        {

            var reply = context.MakeMessage();
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            AdaptiveCards.AdaptiveCard card1 = new AdaptiveCards.AdaptiveCard();
            card1.Body.Add(new TextBlock()
            {
                Text = "Welcome to Adaptive card demo",
                Size = TextSize.Large,
                Weight = TextWeight.Bolder
            });

            // Create the attachment with adapative card.  
            Attachment attachment1 = new Attachment()
            {
                ContentType = AdaptiveCards.AdaptiveCard.ContentType,
                Content = card1
            };
            reply.Attachments.Add(attachment1);
            await context.PostAsync(reply);
        }
    }
}