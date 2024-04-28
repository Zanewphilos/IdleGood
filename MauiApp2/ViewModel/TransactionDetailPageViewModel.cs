using System;
using System.ComponentModel;
using MauiApp2.Model.TransactionDto;
using Microsoft.Maui.Controls; // For ICommand
using System.Windows.Input;
using MauiApp2.Services;
using System.Diagnostics;

namespace MauiApp2.ViewModel
{
    internal class TransactionDetailPageViewModel : BindableObject
    {
        private TransactionHistoryDto _transaction;

        private readonly ApiService_Transaction _apiService_Transaction;

        public event EventHandler RequestUpdateStatus;

        // Property to hold the transaction details
        public TransactionHistoryDto Transaction
        {
            get => _transaction;
            set
            {
                _transaction = value;
                OnPropertyChanged();
            }
        }
        public event EventHandler<string> ErrorOccurred;

        private void OnErrorOccurred(string message)
        {
            ErrorOccurred?.Invoke(this, message);
        }

        public event EventHandler ClosePageRequested;

        protected virtual void OnClosePageRequested()
        {
            ClosePageRequested?.Invoke(this, EventArgs.Empty);
        }
        public ICommand UpdateTransactionStatusCommand { get; private set; }
        public ICommand DeleteTransactionCommand { get; private set; }

        public TransactionDetailPageViewModel(TransactionHistoryDto transaction)
        {
            Transaction = transaction;

            _apiService_Transaction = new ApiService_Transaction();

            UpdateTransactionStatusCommand = new Command(() => RequestUpdateStatus?.Invoke(this, EventArgs.Empty));
            DeleteTransactionCommand = new Command<int>(async (id) => await DeleteUserTransactionAsync(id));
        }



        public async Task UpdateTransactionStatusAsync(int transactionId, TransactionStatusUpdateDto statusUpdateDto)
        {
            try
            {
                bool isSuccess = await _apiService_Transaction.UpdateTransactionStatusAsync(transactionId, statusUpdateDto);
                if (!isSuccess)
                {
                    OnErrorOccurred("Failed to update the transaction status.");
                    
                }
                else
                {
                    OnErrorOccurred("Success to update description.");
                    Transaction.TransactionStatus = statusUpdateDto.Status;
                    OnPropertyChanged(nameof(Transaction));
                }
            }
            catch (Exception ex)
            {
                OnErrorOccurred($"An error occurred: {ex.Message}");
            }
        }
        private async Task DeleteUserTransactionAsync(int transactionId)
        {
            Debug.WriteLine($"Deleting transaction with ID: {transactionId}");
            try
            {
                bool isSuccess = await _apiService_Transaction.DeleteUserTransactionAsync(transactionId);
                if (isSuccess)
                {
                    OnClosePageRequested(); // 触发关闭页面的事件
                }
                else
                {
                    OnErrorOccurred("Failed to delete the transaction.");
                }
            }
            catch (Exception ex)
            {
                OnErrorOccurred($"An error occurred: {ex.Message}");
            }
        }
        protected override void OnPropertyChanged(string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);
        }
    }
}