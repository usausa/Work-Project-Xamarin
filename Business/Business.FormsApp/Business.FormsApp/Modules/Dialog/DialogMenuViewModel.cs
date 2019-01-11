﻿namespace Business.FormsApp.Modules.Dialog
{
    using System.Threading.Tasks;

    using Business.FormsApp.Components.Dialogs;
    using Business.FormsApp.Components.Popup;

    using Smart.Forms.Components;
    using Smart.Forms.Input;
    using Smart.Navigation;

    public class DialogMenuViewModel : AppViewModelBase
    {
        private readonly IDialogService dialogService;

        private readonly IDialog dialog;

        private readonly IPopupNavigator popupNavigator;

        public AsyncCommand<ViewId> ForwardCommand { get; }

        public AsyncCommand ProgressCommand { get; }
        public AsyncCommand LoadingCommand { get; }
        public AsyncCommand DateCommand { get; }
        public AsyncCommand TimeCommand { get; }

        public AsyncCommand PopupCommand { get; }

        public DialogMenuViewModel(
            ApplicationState applicationState,
            IDialogService dialogService,
            IDialog dialog,
            IPopupNavigator popupNavigator)
            : base(applicationState)
        {
            this.dialogService = dialogService;
            this.dialog = dialog;
            this.popupNavigator = popupNavigator;

            ForwardCommand = MakeAsyncCommand<ViewId>(x => Navigator.ForwardAsync(x));
            ProgressCommand = MakeAsyncCommand(Progress);
            LoadingCommand = MakeAsyncCommand(Loading);
            DateCommand = MakeAsyncCommand(Date);
            TimeCommand = MakeAsyncCommand(Time);
            PopupCommand = MakeAsyncCommand(Popup);
        }

        protected override Task OnNotifyBackAsync()
        {
            return Navigator.ForwardAsync(ViewId.Menu);
        }

        private async Task Progress()
        {
            using (var progress = dialog.Progress("Test"))
            {
                for (var i = 0; i < 100; i++)
                {
                    await Task.Delay(50);

                    progress.Update(i + 1);
                }
            }
        }

        private async Task Loading()
        {
            using (dialog.Loading("Test"))
            {
                await Task.Delay(3000);
            }
        }

        private async Task Date()
        {
            var result = await dialog.Date("Test");
            if (result.Ok)
            {
                await dialogService.DisplayAlert("Result", result.Value.ToString("yyyy/MM/dd"), "ok");
            }
        }

        private async Task Time()
        {
            var result = await dialog.Time("Test");
            if (result.Ok)
            {
                await dialogService.DisplayAlert("Result", result.Value.ToString(@"hh\:mm"), "ok");
            }
        }

        private async Task Popup()
        {
            var result = await popupNavigator.PopupAsync<string, bool>(PopupId.DialogPopup, "Test");

            await dialogService.DisplayAlert("Result", result.ToString(), "ok");
        }
    }
}
