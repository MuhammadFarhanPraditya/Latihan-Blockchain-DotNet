using System;
using Nethereum.Web3;

try
{
    var web3 = new Web3("http://127.0.0.1:8545");

    string accountAddress = "0xf39fd6e51aad88f6f4ce6ab8827279cfffb92266"; 

    // Membaca saldo akun secara asinkron (dalam satuan Wei)
    var balanceWei = await web3.Eth.GetBalance.SendRequestAsync(accountAddress);

    // Mengonversi saldo dari Wei ke Ether
    var balanceEther = Web3.Convert.FromWei(balanceWei.Value);

    // Cetak hasil saldo ke layar
    Console.WriteLine($"Alamat: {accountAddress}");
    Console.WriteLine($"Saldo: {balanceEther} ETH");
}
catch (Exception ex)
{
    // Penanganan error jika Hardhat belum berjalan atau URL RPC salah
    Console.WriteLine($"Terjadi kesalahan saat menghubungkan ke node blockchain:");
    Console.WriteLine($"Pesan Error: {ex.Message}");
}