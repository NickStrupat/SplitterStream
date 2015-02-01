using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SplitterStreams {
	public class SplitterStream : Stream {
		private readonly Stream[] destinations;
		public SplitterStream(params Stream[] destinations) {
			this.destinations = destinations;
		}
		public override void Flush() {
			foreach (var destination in destinations)
				destination.Flush();
		}
		public override Int64 Seek(Int64 offset, SeekOrigin origin) {
			throw new InvalidOperationException();
		}
		public override void SetLength(Int64 value) {
			throw new InvalidOperationException();
		}
		public override Int32 Read(Byte[] buffer, Int32 offset, Int32 count) {
			throw new InvalidOperationException();
		}
		public override void Write(Byte[] buffer, Int32 offset, Int32 count) {
			foreach (var destination in destinations)
				destination.Write(buffer, offset, count);
		}
#if NET45
		public override Task WriteAsync(Byte[] buffer, Int32 offset, Int32 count, CancellationToken cancellationToken) {
			return Task.WhenAll(destinations.Select(destination => destination.WriteAsync(buffer, offset, count, cancellationToken)));
		}
#endif
		public override Boolean CanRead {
			get { return false; }
		}
		public override Boolean CanSeek {
			get { return false; }
		}
		public override Boolean CanWrite {
			get { return destinations.All(x => x.CanWrite); }
		}
		public override Int64 Length {
			get { throw new InvalidOperationException(); }
		}
		public override Int64 Position {
			get { throw new InvalidOperationException(); }
			set { throw new InvalidOperationException(); }
		}
	}
}
