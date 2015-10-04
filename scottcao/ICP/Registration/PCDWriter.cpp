#include "PCDWriter.h"

PCDWriter::PCDWriter () : 
	file_name_ ("ICP"),
	file_count_ (0)
{}

PCDWriter::PCDWriter (std::string fn) : 
	file_name_ (fn),
	file_count_ (0)
{}

void PCDWriter::setFileName (std::string fn)
{
	file_name_ = fn;
}

void PCDWriter::writeToFile (PointCloud::Ptr cloud)
{
  ++ file_count_;
  std::stringstream sstream;
  sstream << "Results/" << file_name_ << "-" << file_count_ << ".pcd";
  std::string pcd_name = sstream.str();

  if (pcl::io::savePCDFile(pcd_name, *cloud, true) == 0)
  {
    PCL_INFO("Saved %s.\n", pcd_name.c_str());
    saveFileName (pcd_name);
  }
}

void PCDWriter::saveFileName (std::string file)
{
  stream << file << " ";
}

void PCDWriter::saveText ()
{
  PCL_INFO ("Files Saved: \n%s\n", stream.str().c_str());
  std::ofstream output_text;
  output_text.open ((file_name_+".txt").c_str());
  output_text << stream.str() << std::endl;
  output_text.close();
}