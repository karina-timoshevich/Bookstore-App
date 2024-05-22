using Bookstore_OOP.Model;
using System;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Web;
using Bookstore_OOP.Services;
namespace Bookstore_OOP.View;


[QueryProperty(nameof(Url), "Url")]
public partial class TestPayment : ContentPage
{
    string _url;
    WebView webView;
    DatabaseService dbService = new DatabaseService();


    public async Task CheckPayment()
    {
        var client = new HttpClient();
        var byteArray = Encoding.ASCII.GetBytes("390747:test_hY31ddxSx520A4ARG0FcCqu0fbeKhDdVgtkiauX26TM");
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));


        Uri uri = new Uri(_url);
        string query = uri.Query;
        var queryParams = HttpUtility.ParseQueryString(query);
        string paymentId = queryParams["orderId"];

        var response = await client.GetAsync($"https://api.yookassa.ru/v3/payments/{paymentId}");
        if (response.IsSuccessStatusCode)
        {
            var content_with_payment = await response.Content.ReadAsStringAsync();
            var options_2 = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            Payment payment = JsonSerializer.Deserialize<Payment>(content_with_payment, options_2);
            Debug.WriteLine(content_with_payment);
            //Debug.WriteLine(payment.Confirmation.Confirmation_url);

            Debug.WriteLine(payment.Status);   
            if (payment.Status == "succeeded")
            {
                Debug.WriteLine("Payment succeeded");
                dbService.DeleteCartItemsByUserId(dbService.GetCurrentUser());

            }
            else
            {
                Debug.WriteLine("Payment failed");
                dbService.DeleteLastUserOrder(dbService.GetCurrentUser());
            }

        }
        else
        {
            Debug.WriteLine(response.StatusCode.ToString());
        }

    }
    public string Url
    {
        set
        {
            _url = Uri.UnescapeDataString(value.Trim());
            Debug.WriteLine("Url: " + _url);
            OnPropertyChanged();

            webView = new WebView
            {
                Source = _url,
            };
            webView.Navigating += (s, e) =>
            {
                if (e.Url.StartsWith("https://github.com/karina-timoshevich"))
                {
                    CheckPayment();
                    e.Cancel = true;

                    
                    Shell.Current.GoToAsync("//UserOrders");

                   
                }
            };
            Content = webView;
        }

        get => _url;
    }

    public TestPayment()
    {
        dbService.InitDB();
        Debug.WriteLine("TestPayment");
        Debug.Write("Plain text");
        Debug.WriteLine(Url);
    }
}