using System;
using System.Collections.Generic;
using Astryx.Abstractions.Agents;
using Astryx.Abstractions.Exceptions;
using Astryx.Abstractions.Substrate.Exporting;

namespace Astryx.Hub.Substrate.Helpers
{
    public static class ExportBundleNormalizer
    {
        public static AgentExportBundle Normalize(AgentExportBundle bundle)
        {
            if (bundle.Mind == null)
                throw new InvalidOperationException("Export bundle missing MindConfig.");

            if (bundle.Persona == null)
                throw new InvalidOperationException("Export bundle missing PersonaExport.");

            if (bundle.Provenance == null)
                bundle.Provenance = Array.Empty<IProvenanceEntry>();

            return bundle;
        }
    }
}