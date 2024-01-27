using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace OutsourceTracker.Data.Csv;

public class CsvRepository<TItem, TMap>(string filePath) : IDisposable
    where TMap : ClassMap
{
    private CsvReader? _csvReader;
    private bool _disposed;
    private StreamReader? _streamReader;

    public string FilePath { get; } = filePath;

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public IEnumerable<TItem> GetItems()
    {
        ThrowIfDisposed();
        InitReader();
        return _csvReader!.GetRecords<TItem>();
    }

    private void InitReader()
    {
        _streamReader = new StreamReader(FilePath);
        _csvReader = new CsvReader(_streamReader, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = false
        });
        _csvReader.Context.RegisterClassMap<TMap>();
    }

    private void ThrowIfDisposed()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _streamReader?.Dispose();
                _csvReader?.Dispose();
            }

            _disposed = true;
        }
    }
}